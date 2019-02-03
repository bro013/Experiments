import numpy as np

phi = (1 + np.sqrt(5))/2.0
phi_ = (1 - np.sqrt(5))/2.0

F = lambda n:(np.power(phi, n) - np.power(phi_, n))/np.sqrt(5.0)

i = 100
X = np.arange(i)
N = 0
L = 4e6


for x in X:
    if F(x) > L:
        N = x.copy() - 1
        break
    if F == L:
        N = x.copy()
        break


if N > 0:
    S = np.sum(F(np.arange(3, N + 1, 3)))
    print(S)
else:
    print("Solution not found whithin {0} steps".format(i))