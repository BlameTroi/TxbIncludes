\ test-TxbWords.fs -- My common definitions -- T.Brumley

   marker TEST-TXBWORDS

   require TxbUnitTesting.fs
   require TxbWords.fs

unit-test-reset

\ sum should be correct, stack should be clean

unit-test-stack?
1 2 3 3dup + + + + + s" 3dup 1 2 3" 12 unit-test-n
unit-test-stack?

\ characters are single bytes, this is a silly test

unit-test-stack?
0 char+ s" char+ + 1" 1 unit-test-n
32 chars s" chars * 1" 32 unit-test-n
unit-test-stack?

\ within vs within?

unit-test-stack?
1 0 10 within s" 1 within 0 10" true unit-test-bool
0 0 10 within s" 0 within 0 10" true unit-test-bool
-1 0 10 within s" -1 within 0 10" false unit-test-bool
9 0 10 within s" 9 within 0 10" true unit-test-bool
10 0 10 within s" 10 within 0 10" false unit-test-bool
11 0 10 within s" 11 within 0 10" false unit-test-bool
unit-test-stack?

unit-test-stack?
1 0 10 within? s" 1 within? 0 10" true unit-test-bool
0 0 10 within? s" 0 within? 0 10" true unit-test-bool
-1 0 10 within? s" -1 within? 0 10" false unit-test-bool
9 0 10 within? s" 9 within? 0 10" true unit-test-bool
10 0 10 within? s" 10 within? 0 10" true unit-test-bool
11 0 10 within? s" 11 within? 0 10" false unit-test-bool
unit-test-stack?

\ report

unit-test-report
unit-test-reset

\ End of test-TxbWords.fs
