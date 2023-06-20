
"strict";

fetch(browser.runtime.getURL("src/origin-demangler.wasm"))
    .then(response => response.arrayBuffer())
    .then(bytes => WebAssembly.instantiate(bytes, {}))
    .then(wasm => {
        console.log("wasm loaded");
        wasm.instance.exports.main();
    })
    .catch(err => {
        console.error("could not load wasm", err)
    });
