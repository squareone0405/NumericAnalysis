#pragma once

double get_pix(double pi, int x) {
	double pi_x = 1.0;
	for (int i = 0; i < x; ++i)
		pi_x *= pi;
	return pi_x;
}

double get_pix_rk5(double xlnpi, int n) {
	double h = (xlnpi / n);
	double yn = 1.0;
	double k = 1 + h * (1 + h / 2 * (1 + h / 3 * (1 + h / 4 * (1 + h / 5 * (1 + h / 6)))));
	for (int i = 0; i < n; ++i) {
		yn *= k;
	}
	return yn;
}

double get_ex_taylor(double x, int round) {
	double e_x = 1.0;
	double temp = 1.0;
	for (int i = 1; i < round; ++i) {
		temp *= (x / i);
		e_x += temp;
	}
	return e_x;
}