# Troy's Miscellaneous Forth Includes

## Project Summary

I'm (re)learning Forth for this year's _Advent of Code_ and need
a place to collect utility words without worrying about proper
library organization. Each include file will tend toward a theme
or focus.

## Which Forth

My requirements were smallish, reasonably ANS compliant, and
buildable on Apple Silicon. Even with that last requirement there
were options (`gforth`, `DuskOS`, `Min3rd`, `pforth`, ...).

> You are in a maze of twisty little languages, all (almost)
> alike.

After experimenting I settled on
[`pforth`](https://www.softsynth.com/pforth/)
([repository](https://github.com/philburk/pforth/)). It builds on
an ARM Mac and runs well.

The only obvious area of incompatibility is file inclusion and
compilation. Includes nest and the `anew`/`include?` mechanism
works well as a `#pragma once` alternative.

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
