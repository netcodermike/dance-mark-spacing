using System.Text.Json;

try
{
    if (args.Length != 1) throw new ArgumentException("Pass one JSON input file");
    using var data = JsonDocument.Parse(File.ReadAllText(args[0]));
    Console.WriteLine(JsonSerializer.Serialize(Analyze(data.RootElement)));
    return 0;
}
catch (Exception error) when (error is IOException or JsonException or FormatException
                               or ArgumentException or InvalidOperationException
                               or KeyNotFoundException or OverflowException)
{
    Console.Error.WriteLine(error.Message);
    return 2;
}

static object Analyze(JsonElement data)
{
    var minimum=data.GetProperty("minimum").GetDouble();
    if(!double.IsFinite(minimum)||minimum<=0)throw new ArgumentException("Invalid minimum spacing");
    var marks=new List<(string name,double x,double y)>();
    var names=new HashSet<string>();
    foreach(var mark in data.GetProperty("marks").EnumerateArray())
    {
        if(mark.GetArrayLength()!=3)throw new ArgumentException("Expected a label and two coordinates");
        var name=mark[0].GetString()??"";
        var x=mark[1].GetDouble();
        var y=mark[2].GetDouble();
        if(string.IsNullOrWhiteSpace(name)||!names.Add(name)||!double.IsFinite(x)||!double.IsFinite(y)||Math.Abs(x)>1e100||Math.Abs(y)>1e100)
            throw new ArgumentException("Invalid marker");
        marks.Add((name,x,y));
    }
    double? closest=null;
    var closePairs=new List<string[]>();
    for(var i=0;i<marks.Count;i++)
        for(var j=i+1;j<marks.Count;j++)
        {
            var a=marks[i];
            var b=marks[j];
            var distance=Math.Sqrt(Math.Pow(a.x-b.x,2)+Math.Pow(a.y-b.y,2));
            closest=closest.HasValue?Math.Min(closest.Value,distance):distance;
            if(distance<minimum)closePairs.Add(new[]{a.name,b.name});
        }
    return new {closest,closePairs};
}
