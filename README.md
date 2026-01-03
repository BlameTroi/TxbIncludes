# Troy's Miscellaneous Forth Includes

## Project Summary

I'm (re)learning Forth for this year's _Advent of Code_ and need a
place to collect utility words without worrying about proper library
organization. Each include file will have a theme or focus.

## Contents

| File                  | Description                          |
| :-------------------- | :----------------------------------- |
| `TxbAnsiTerminal.fs` | Sends ANSI terminal control codes    |
| `TxbStrings.fs`      | String related definitions           |
| `TxbUnitTesting.fs`  | Unit test harness                    |
| `TxbWords.fs`        | Definitions that don't fit elsewhere in code |

Plus testing files. These are prefixed with "test-".

The testing files are self contained. Each requires the unit test
harness and the definitions it tests.

The preamble for a testing file is:

```Forth
   marker TEST-TXB??????

   require TxbUnitTesting.fs
   Require Txb??????.fs

   unit-test-reset
```

## Which Forth and How I Forth

My requirements are a smallish, reasonably ANS compliant, and
buildable on Apple Silicon. Even with that last requirement there
were options (`gforth`, `DuskOS`, `Min3rd`, `pforth`, ...).

> You are in a maze of twisty little languages, all (almost)
> alike.

After experimenting I settled on[`gforth`](https://gforth.org/)(
[repository](https://git.savannah.gnu.org/git/gforth)). I was able
to build 0.7.9_20251203 on an ARM Mac and it runs well. `pforth` is
excellent but it doesn't have some of the standard words that I
want.

require may be the way to handle dependencies, but I'm not certain.
I'll figure it out if I ever write anything I want to package and
distribute.

## License

My code is all public domain. If you want something more explicit,
pick from either the MIT or the UNLICENSE as in the LICENSE file.

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

Troy Brumley\
BlameTroi@gmail.com\
So let it be written...\
...so let it be done
