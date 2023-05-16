# Rust Testing

## Why we need this
Managing gamestate with [NIF resources](https://www.erlang.org/doc/man/erl_nif.html#functionality)
has made testing from Rust itself (i.e. `cargo test`) not quite simple, as we need a running instance of the BEAM.
There is also the technical limitation of [erlang:load_nif/2](https://www.erlang.org/doc/man/erlang.html#load_nif-2) that
only allows to load NIFs on a single module by OTP app, so we can't simple
have separate modules by mix environment.
To test rust code itself, this separate OTP app was made, with the exclusive purpose
of calling rust tests and manage them. [This is is also what the rustler team does](https://github.com/rusterlium/rustler/tree/d4e0a7bd2bc8e6e90bc56cc5b7ee4faedd6fa84a/rustler_tests)

## Adding tests

1. Define your test under the crate rustler_test as a plain rust function
   with a NIF macro decorator, like in this example:
  ```rust
  use crate::assert_result;
  use crate::utils::TestResult;
  #[rustler::nif]
  pub fn math_works() -> TestResult {
     let x = 1;
     assert_result!(x-1, 0)?; // Note the question sign, as we return result.
     assert_result!(x+1, 2)
  }
  ``` 
  This can actually be more complex and call any function
  from the gamestate crate, you shouldn't have problems accessing NIF
  resources, if you do, please report it.
  You can check more complex test cases on this file: `native/rustler_test/src/game_state_test.rs`

2. Add your declared NIF under `rustler::init!`

3. Next, you'll need to add your function to `lib/test_nifs.ex`:
  ```elixir
  defmodule TestNIFs do
    def math_works(), do: :erlang.nif_error(:nif_not_loaded)
  end
  ``` 

4. Use it as a test under `test/test_file.exs`:
  ```elixir
  defmodule MyTestingModule do
    def math_works(), do: :erlang.nif_error(:nif_not_loaded)
  end
  ``` 

5. Finally, run mix test.

### About asserts
 You can actually use any assert-like macro, but this will only be
 reported as nif panic and won't give too much information about the failing
 test. That's why assert_result! is used. Were the test to fail, you'll get
 a message detailing how and where the test has failed.

 
  
