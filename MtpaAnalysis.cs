namespace MtpaAnalysis;

public class MySolver(double lambda, double l_d, double l_q, double p)
{
    private readonly double _p = p;

    private readonly double _lambda = lambda;
    private readonly double _l_d = l_d;
    private readonly double _l_q = l_q;
    // private readonly double _max_torque = max_torque;

    public double Lambda => _lambda;
    public double L_d => _l_d;
    public double L_q => _l_q;
    public double P => _p;

    public double Id(double iq)
    {
        return -Lambda / (2 * (L_q - L_d)) + Math.Sqrt(Lambda * Lambda / (4 * (L_q - L_d) * (L_q - L_d)) + iq * iq);
    }

    public double Iq(double Te)
    {
        double numerator = 8 * Te * Lambda / (3 * P) + Math.Sqrt(64 * Te * Te * Lambda * Lambda / (9 * P * P) - 4 * (Lambda * Lambda - 4 * (L_d - L_q) * (L_d - L_q)) * (16 * Te * Te / 9 * P * P - Lambda * Lambda));
        double denominator = 2 * (Lambda * Lambda - 4 * (L_d - L_q) * (L_d - L_q));
        return numerator / denominator;
    }
}