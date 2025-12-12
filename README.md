# Troy's Miscellaneous Forth Includes

## Project Summary

I'm (re)learning Forth for this year's _Advent of Code_ and need
a place to collect utility words without worrying about proper
library organization. Each include file will have a theme or
focus.

## Contents

| File                  | Description                          |
| :-------------------- | :----------------------------------- |
| `TxbAnsiTerminal.fth` | Sends ANSI terminal control codes    |
| `TxbStrings.fth`      | String related definitions           |
| `TxbUnitTesting.fth`  | Unit test harness                    |
| `TxbWords.fth`        | Definitions that don't fit elsewhere |

Plus testing files. These are prefixed with "test-".

The testing files are self contained. Each REQUIREs the unit test
harness and the definitions it tests.

The preamble for a testing file is:

```Forth
   MARKER TEST-TXB??????

   REQUIRE TxbUnitTesting.fth
   Require Txb??????.fth

   unit.test.reset
```

## Which Forth and How I Forth

My requirements are a smallish, reasonably ANS compliant, and
buildable on Apple Silicon. Even with that last requirement there
were options (`gforth`, `DuskOS`, `Min3rd`, `pforth`, ...).

> You are in a maze of twisty little languages, all (almost)
> alike.

After experimenting I settled on
[`pforth`](https://www.softsynth.com/pforth/)
([repository](https://github.com/philburk/pforth/)). It builds on
an ARM Mac and runs well. I've taken to running batch tests
against `gforth` just to make sure I'm not drifting away from
reasonable standards.

~~The only obvious area of incompatibility is file inclusion and
compilation. Includes nest and the `anew`/`include?` mechanism
works well as a `#pragma once` alternative.~~

REQUIRE looks as if it will handle the dependencies between
included files properly. It has the advantage of being both easy
and portable. At least between `pforth` and `gforth`.

Unfortunately, reloading a test does not reload the files being
tested. I will continue to seek a good solution.

## License

My code is all public domain. If you want something more
explicit, pick from either the MIT or the UNLICENSE as in the
LICENSE file.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

Troy Brumley\
BlameTroi@gmail.com\
So let it be written...\
...so let it be done
