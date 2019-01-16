#include <stdio.h>
#include <stdlib.h>
#include "calc_pi.h"
#include "calc_lnpi.h"
#include "calc_pix.h"

int main() {
	printf("Please input x: \n");
	double x;
	scanf_s("%lf", &x);
	x = x > 10.0 ? 10 : x;
	x = x < 1.0 ? 1.0 : x;
	int x_integer = (int)x;
	double x_decimals = x - x_integer;
	printf("---------------------------------------\n");
	printf("pi:\n");
	printf("arctan1(50):\t%.14f\n", get_pi_taylor_arctan(50));
	printf("arcsin1/2(23):\t%.14f\n", get_pi_taylor_arcsin(23));
	double pi = get_pi_bbp(11);
	printf("bbp(11):\t%.14lf\n", pi);
	printf("---------------------------------------\n");
	printf("ln(pi):\n");
	printf("simpson(8192):\t%.14lf\n", get_lnpi_integral_simpson(pi, 8192));
	printf("cotes(256):\t%.14lf\n", get_lnpi_integral_cotes(pi, 256));
	double lnpi = get_lnpi_newton(pi, 1.0, 5);
	printf("newton(5):\t%.14lf\n", lnpi);
	printf("---------------------------------------\n");
	printf("pi^x:\n");
	double epi_x_decimals = get_ex_taylor(lnpi * x_decimals, 19);
	double epi_x_integer = get_pix(pi, x_integer);
	printf("rk5(256):\t%.14lf\n", epi_x_integer*get_pix_rk5(lnpi * x_decimals, 256));
	printf("taylor(19):\t%.14lf\n", epi_x_decimals * epi_x_integer);
	system("pause");
	return 0;
}
