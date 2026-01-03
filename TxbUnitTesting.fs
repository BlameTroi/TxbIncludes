\ TxbUnitTesting.fs -- Simple unit testing -- T.Brumley

   marker TXBUNITTESTING

   require TxbWords.fs

   require TxbAnsiTerminal.fs

\ A slowly growing minimalistic unit testing vocabulary.
\
\ I expect to include this file in my other includes to run
\ tests after loading. Counts for passed, failed, and errored
\ tests can be printed.
\
\ To use, include? this file in your source. Then toward the
\ end of your source run your tests.
\
\    unit-test-reset \ zero counters
\    ( your tests here )
\    unit-test-report \ prints totals
\    unit-test-reset
\ 
\ The general structure of a test case is:
\
\    do something that places a result on the stack
\    s" some text describing the test"
\    place the expected value on the stack
\    unit-test-????
\
\ inputs predicate s" msg" expected unit-test-<which>
\ 
\ unit-test-bool is meant to verify predicates.
\
\ `3 odd? s" is 3 odd?" true unit-test-bool`       -- pass.
\ `4 odd? s" deliberate fail" true unit-test-bool` -- fail.
\ 

\ Reporting

variable unit-passed       ( how many have we run )
variable unit-failed       ( tests that did not pass )
variable unit-errored      ( not used yet, errors/exceptions )
variable unit-stack        ( count stack not cleared )

\ Empty the stack after checking for dangling items. Prints
\ each item if any are left while clearing.

: unit-test-stack? ( -- )
   depth if
      cr s" Stack not cleared: " type-red depth . cr
      depth 0 ?do cr . loop cr
      unit-stack dup @ 1+ swap !
   then ;

: unit-test-reset ( -- )
   unit-test-stack?
   0 unit-stack !
   0 unit-passed !
   0 unit-failed !
   0 unit-errored ! ;

: unit-test-report ( -- )
   cr ." Tests" cr
   s" Failed " type-red unit-failed @ . cr
   s" Passed " type-green unit-passed @ . cr
   s" Errored " type-cyan unit-errored @ . cr
   ." Total of " unit-failed @ unit-passed @ + . ." tests." cr
   unit-stack @ if
      s" Times the stack was not cleared " type-red unit-stack @ . cr
   then ;
    
: unit-test-passed ( -- )
   unit-passed dup @ 1+ swap ! ;

: unit-test-failed ( -- )
   unit-failed dup @ 1+ swap ! ;

: unit-test-errored ( -- )
   unit-errored dup @ 1+ swap ! ;

\ Convert flag to string

: unit-as-bool ( flag -- str len , for reporting )
   if   s" True"
   else s" False"
   then ;

\ evaluate and report test results

\ unit-test-bool compares results as booleans. The results
\ are forced to -1 for true and 0 for false.

: unit-test-bool ( got str len wanted -- , report result )
   -rot cr ." Test: " type space   \ got wanted --
   0= 0= swap 0= 0= swap           \ to true false --
   2dup                            \ g w g w --
   = if
      unit-test-passed             \ g w -- , count passed
      s" passed" type-green
      2drop                        \ discard
   else
      unit-test-failed             \ g w -- , count failed
      s" FAILED " type-red
      ." wanted " unit-as-bool type \ g -- , print wanted
      ."  got " unit-as-bool type   \   -- , and got
   then ;

\ unit-test-n compares results as integers.

: unit-test-n ( got str len wanted -- , report result )
   -rot cr ." Test: " type space   \ got wanted --
   2dup                            \ g w g w --
   = if
      unit-test-passed             \ g w -- , count passed
      s" passed" type-green
      2drop                        \ discard
   else
      unit-test-failed             \ g w -- , count failed
      s" FAILED " type-red
      ." wanted " .                \ g -- , print wanted
      ."  got " .                  \   -- , and got
   then ;

\ unit-test-reset
\ cr ." should pass: " true  s" passing" true unit-test-bool
\ cr ." should fail: " false s" failing" true unit-test-bool
\ cr
\ unit-test-report
\ unit-test-reset
\ cr

\ End of TxbUnitTesting.fs
