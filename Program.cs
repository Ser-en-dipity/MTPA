// See https://aka.ms/new-console-template for more information
using System.Text;
using MtpaSolver;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

Console.WriteLine("Hello, World!");


var Lambda = 0.1192;
var L_d = 0.0005;
var L_q = 0.0011;
var P = 4;

var solver = new MySolver(Lambda, L_d, L_q, P);

var max_torque = 50;
var step = 0.1;

var i = solver.SolveForDq(max_torque, step);
// dump to csv file

var sb = new StringBuilder();
sb.AppendLine("i_d,i_q,torque");

for (var j = 0; j < i.Length; j++)
{
    sb.AppendLine($"{i[j].Item1},{i[j].Item2},{step * j}");
}

File.WriteAllText("result.csv", sb.ToString());
Console.WriteLine("Done!");


var ana = new MtpaAnalysis.MySolver(Lambda, L_d, L_q, P);
for (double te = 0; te < 50; te += 0.1)
{
    var iq = ana.Iq(te);
    var id = ana.Id(iq);
    Console.WriteLine($"Te: {te}, Id: {id}, Iq: {iq}");
}





