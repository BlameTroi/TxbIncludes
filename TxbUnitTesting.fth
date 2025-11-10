\ TxbUnitTesting.f -- Simple unit testing -- T.Brumley

include? task-TxbAnsiTerminal.fth TxbAnsiTerminal.fth
anew task-TxbUnitTesting.fth

\ A slowly growing minimalistic unit testing vocabulary.
\
\ I expect to include this file in my other includes to (re)run
\ tests after loading. Counts for passed, failed, and errored
\ tests can be printed.
\
\ To use, include? this file in your source. Then toward the end
\ of your source run your tests.
\
\    unit.test.reset \ zero counters
\    ( your tests here )
\    unit.test.report \ prints totals
\    unit.test.reset
\ 
\ The general structure of a test case is:
\
\    do something that places a result on the stack
\    s" some text describing the test"
\    place the expected value on the stack
\    unit.test.????
\ 
\ So far there is only unit.test.bool which is meant to verify
\ predicates. For example, assuming you have a word odd? defined:
\
\ `3 odd? s" is 3 odd?" true unit.test.bool` should pass.
\ `4 odd? s" deliberate fail" true unit.test.bool` should fail.
\
\ inputs predicate s" msg" expected unit.test.boolean?


\ Statistics

variable unit.passed       \ how many have we run
variable unit.failed       \ tests that did not pass
variable unit.errored      \ not used yet, errors/exceptions

: unit.test.reset ( -- )
    0 unit.passed !
    0 unit.failed !
    0 unit.errored ! ;

: unit.test.report ( -- )
    cr ." Tests" cr
    s" Failed " type.red unit.failed @ . cr
    s" Passed " type.green unit.passed @ . cr
    s" Errored " type.cyan unit.errored @ . cr
    ." Total of " unit.failed @ unit.passed @ + . ." tests." cr ;
    
: unit.test.passed ( -- )
    unit.passed dup @ 1+ swap ! ;

: unit.test.failed ( -- )
    unit.failed dup @ 1+ swap ! ;

: unit.test.errored ( -- )
    unit.errored dup @ 1+ swap ! ;


\ Convert flag to string

: unit.as.bool ( flag -- str len , for reporting )
    if   s" True"
    else s" False"
    then ;

\ evaluate and report test results

: unit.test.bool ( got str len wanted -- , report test result )
    -rot cr ." Test: " type space    \ leaves got wanted
    not not swap not not swap        \ canonical got wanted
    2dup = if
        unit.test.passed             \ got the expected result 
        s" passed" type.green
        2drop
    else
        unit.test.failed
        s" FAILED " type.red
        ." wanted " unit.as.bool type
        ."  got " unit.as.bool type
    then ;

unit.test.reset

cr ." this should pass: " true  s" passing" true unit.test.bool
cr ." this should fail: " false s" failing" true unit.test.bool
cr

unit.test.report

unit.test.reset

\ End of TxbUnitTesting.fth
