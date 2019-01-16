#pragma once

#include "calc_pix.h"

double get_lnpi_integral_simpson(double pi, int n) {
	double pi1 = pi - 1;
	double h = pi1 / n;
	double h2 = h / 2;
	double lnpi = -1.0;
	lnpi += 1.0 / pi;
	for (int i = 0; i < n; ++i) {
		lnpi += 2.0 / (1.0 + 2 * i*h2);
		lnpi += 4.0 / (1.0 + (2 * i + 1)*h2);
	}
	lnpi *= (h / 6.0);
	return lnpi;
}

double get_lnpi_integral_cotes(double pi, int n) {
	double pi1 = pi - 1;
	double h = pi1 / n;
	double h4 = h / 4;
	double lnpi = -7.0;
	lnpi += 7.0 / pi;
	for (int i = 0; i < n; ++i) {
		lnpi += 14.0 / (1.0 + 4 * i*h4);
		lnpi += 32.0 / (1.0 + (4 * i + 1)*h4);
		lnpi += 12.0 / (1.0 + (4 * i + 2)*h4);
		lnpi += 32.0 / (1.0 + (4 * i + 3)*h4);
	}
	lnpi *= (h / 90.0);
	return lnpi;
}

/*double get_lnpi_integral_cotes(double pi, int n) {
	return get_lnpi_integral_simpson(pi, 2 * n) * 16 / 15 - get_lnpi_integral_simpson(pi, n) / 15;
}*/

double get_lnpi_integral_romberg(double pi, int n) {
	return get_lnpi_integral_cotes(pi, 2 * n) * 64 / 63 - get_lnpi_integral_cotes(pi, n) / 63;
}

double get_lnpi_newton(double pi, double x0, int n) {
	double y = x0;
	for (int i = 0; i < n; ++i) {
		y = y + pi / get_ex_taylor(y, 18) - 1;
	}
	return y;
}
