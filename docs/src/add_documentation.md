# How to add documentation

## Install mdbook

☝️🤓 First make sure you have installed mdbook in your machine. Run `mdbook -V`. If you have it you will see something like `mdbook v0.4.34` if you don't, install it with the following command lines:

```
cargo install mdbook
cargo install mdbook-mermaid
```
Now you are ready to continue, for more information check the mdbook [mdbook docs](https://rust-lang.github.io/mdBook/).

## Create a new section
If you want to add a new documentation page it is pretty simple, follow the steps bellow

### Using the Command Line :
In the root of the project type:
```
cd docs/src
```
Inside the `src` folder create your markdown document (without curly braces).
```
touch {your_page_name}.md
open {your_page_name}.md
```

Write your document and save it. Then open `SUMMARY.md` document and add your new page as following:

```
- [Your new section name](./{your_page_name}.md)
```

Finally go back to the root folder `cd ../..` and run `make docs`, now you are ready to go. Your new section had been added to the docs 🤩
