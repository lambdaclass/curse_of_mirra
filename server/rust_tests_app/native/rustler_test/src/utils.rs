use std::collections::HashMap;
use tinyjson::JsonValue;
pub type TestResult = Result<String, String>;
pub fn read_character_config() -> Vec<HashMap<String, String>> {
    let path = "../client/Assets/StreamingAssets/Characters.json";
    let file = std::fs::read_to_string(path)
        .expect("Missing config file! Make sure you're running this from the server folder");
    let parsed = file
        .parse::<JsonValue>()
        .expect("Could not parse config json!");
    let object: HashMap<_, _> = parsed.try_into().expect("Not valid config in json");
    let characters: Vec<JsonValue> = object["Items"]
        .clone()
        .try_into()
        .expect("Expected an array of characters");
    let info = characters
        .into_iter()
        .map(|character_info| {
            let mut map: HashMap<String, String> = HashMap::new();
            let character_info: HashMap<_, _> = character_info
                .clone()
                .try_into()
                .expect(&format!("Expected object, got: {:?}", character_info));
            for (key, value) in character_info {
                let string: String = value
                    .try_into()
                    .expect("Character json values must be strings!");
                map.insert(key, string);
            }
            return map;
        })
        .collect::<Vec<_>>();
    return info;
}

pub fn read_skills_config() -> Vec<HashMap<String, String>> {
    let path = "../client/Assets/StreamingAssets/Skills.json";
    let file = std::fs::read_to_string(path)
        .expect("Missing config file! Make sure you're running this from the server folder");
    let parsed = file
        .parse::<JsonValue>()
        .expect("Could not parse config json!");
    let object: HashMap<_, _> = parsed.try_into().expect("Not valid config in json");
    let skills: Vec<JsonValue> = object["Items"]
        .clone()
        .try_into()
        .expect("Expected an array of skills");
    let info = skills
        .into_iter()
        .map(|skills_info| {
            let mut map: HashMap<String, String> = HashMap::new();
            let skills_info: HashMap<_, _> = skills_info
                .clone()
                .try_into()
                .expect(&format!("Expected object, got: {:?}", skills_info));
            for (key, value) in skills_info {
                let string: String = value
                    .try_into()
                    .expect("Skills json values must be strings!");
                map.insert(key, string);
            }
            return map;
        })
        .collect::<Vec<_>>();
    return info;
}

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
