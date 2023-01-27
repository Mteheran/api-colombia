# API Documentation HTML Template

### A simple, modern and readable HTML template for APIs documentations

You can take a look with this [DEMO](https://floriannicolas.github.io/API-Documentation-HTML-Template/). \
Or this other [One Content Column DEMO](https://floriannicolas.github.io/API-Documentation-HTML-Template/one-content-column). 

## Current version : 1.0.5


### What's new in the latest version : 

- Fix list on 3 content columns `<ul>` and `<ol>`.
- Removed `jQuery` usage to vanilla js.
- Update of css fonts.
- Fix menu with long text.
- Updated Google Font call.
- Removed usage of `Roboto Condensed` font.
- Updated `menu` `data-target` to use `content`.
- Added `.central-overflow-x` util class to avoid overflows.
- Added `.break-word` util class to avoir overflows without adding a scrollbar.
- Added optional `Version` & `Last updated` infos
- Added responsive menu with `burger icon` menu button. 


## Credits

* Google Font (Roboto|Source+Code+Pro)
* Highlight.js 9.8.0
* A Creative Common logo: platform by Emily van den Heever from the Noun Project.

## How to use it

This is a simple HTML template, do whatever you want with this !

To use One Content Column Version, don't forget to add ```one-content-column-version``` css class to ```<body>``` like in ```one-content-column.html``` file. 

## Utils CSS class 

If you have an element in central column that overflow on third column, you can add it `central-overflow-x` css class to prevent it.

Example: 
```html
<table class="central-overflow-x">...<table>
```

If you doesn't want a scrollbar, you can use `break-word` css class to prevent it.

Example: 
```html
<code class="higlighted break-word">http://api.westeros.com/with-a-very-very-very-very-very-long-end-point-url/get<table>
```


## Contributors

Special thanks to [TheStami](https://github.com/TheStami) for his contribution creating [One Content Column version](https://ticlekiwi.github.io/API-Documentation-HTML-Template/one-content-column) ! 


## Contribute

We're always looking for:

* Bug reports, especially those for aspects with a reduced test case
* Pull requests for features, spelling errors, clarifications, etc.
* Ideas for enhancements
