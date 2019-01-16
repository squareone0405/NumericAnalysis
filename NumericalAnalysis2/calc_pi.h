#pragma once

double get_pi_taylor_arctan(int round) {
	double pi = 2.0;
	double temp = 2.0;
	for (int i = 1; i < round; ++i) {
		temp *= (i / (2 * i + 1.0));
		pi += temp;
	}
	return pi;
}

double get_pi_taylor_arcsin(int round) {
	double pi = 3.0;
	double temp = 3.0;
	for (int i = 1; i < round; ++i) {
		temp *= ((4 * i*i - 4 * i + 1.0) / (16 * i*i + 8 * i));
		pi += temp;
	}
	return pi;
}

double get_pi_bbp(int round) {
	double pi = 0.0;
	unsigned long long num;
	unsigned long long den;
	unsigned long long n16;
	for (long long i = round - 1; i >= 0; --i) {
		num = 120 * i*i + 151 * i + 47;
		den = (2 * i + 1)*(4 * i + 3)*(8 * i + 1)*(8 * i + 5);
		n16 = (unsigned long long)1 << (4 * i);
		pi += ((1.0 * num) / den) / n16;
	}
	return pi;
}
