pub type TestResult = Result<String, String>;
#[macro_export]
macro_rules! assert_result {
    ($expected:expr, $actual:expr) => {
        if $expected != $actual {
            let line = line!();
            let file = file!();
            let msg = format!(
                "Assert failed, expected: {:?}, but got: {:?} on line: {:?}, file: {:?}",
                $expected, $actual, line, file
            );
            Err(msg)
        } else {
            Ok("".to_string())
        }
    };
    ($expected:expr, $actual:expr, $optional_ok_value:expr) => {
        if $expected != $actual {
            let line = line!();
            let file = file!();
            let msg = format!(
                "Assert failed, expected: {:?}, but got: {:?} on line: {:?}, file: {:?}",
                $expected, $actual, line, file
            );
            Err(msg)
        } else {
            Ok(expr)
        }
    };
}
