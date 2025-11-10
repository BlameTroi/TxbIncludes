\ TxbWords.f -- things I haven't found in pforth -- T.Brumley

\ These appear generally useful but don't group naturually
\ into a category like "unit test" or "i/o".

anew task-TxbWords.fth

\ ANS WITHIN is n x y -- flag, x <= n < y, or [x, y )
\ From Programming Forth we get:
\ WITHIN? is (oddly named in my opinion) x <= n <= y, or [x, y]

: within? ( n low high -- flag, closed interval )
    1+ within ;

\ End of TxbWords.fth
