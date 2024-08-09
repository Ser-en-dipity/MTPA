namespace MtpaSolver;

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
	// public double MaxTorque => _max_torque;


	public double Id(double im)
	{
		var k = Lambda / ((L_q - L_d) * 4);
		return k - Math.Sqrt(k * k + im * im / 2);
	}
	public double dId(double im)
	{
		var k = Lambda / ((L_q - L_d) * 4);
		return -0.5 * im / Math.Sqrt(k * k + im * im / 2);
	}
	public double Iq(double im)
	{
		return Math.Sqrt(im * im - Id(im) * Id(im));
	}
	public double dIq(double im)
	{
		return 0.5 / Math.Sqrt(im * im - Id(im) * Id(im)) * (2 * im - 2 * Id(im) * dId(im));
	}
	public double Torque(double im)
	{
		return 1.5 * P * (Lambda * Iq(im) + (L_d - L_q) * Id(im) * Iq(im));
	}

	public double dTorque(double im)
	{
		return 1.5 * P * (Lambda * dIq(im) + (L_d - L_q) * (dId(im) * Iq(im) + Id(im) * dIq(im)));
	}
	public double ImRef(double T)
	{
		return T * 2 / (3 * P * Lambda);
	}

	public double RNIter(double te, double epsilon)
	{
		var i = ImRef(te);

		Func<double, double> update = (double i_n) =>
		{
			var x = Torque(i_n) - te;
			var dx = dTorque(i_n);
			var i_n_z = i_n - x / dx;
			return i_n_z;
		};
		if (te <= 0)
		{
			return 0;
		}

		while (Math.Abs(Torque(i) - te) > epsilon)
		{
			i = update(i);
		}
		return i;
	}

	public double[] Solve(double max_torque, double step = 0.1, double epsilon = 1e-6)
	{
		var result = new List<double>();
		for (var te = 0.0; te <= max_torque; te += step)
		{
			result.Add(RNIter(te, epsilon));
		}
		return result.ToArray();
	}

	public Tuple<double, double>[] SolveForDq(double max_torque, double step = 0.1, double epsilon = 1e-6)
	{
		var result = new List<Tuple<double, double>>();
		for (var te = 0.0; te <= max_torque; te += step)
		{
			var i = RNIter(te, epsilon);
			result.Add(new Tuple<double, double>(Id(i), Iq(i)));
		}
		return result.ToArray();
	}

}
