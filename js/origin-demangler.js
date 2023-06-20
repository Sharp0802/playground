
"strict";

WebAssembly.instantiateStreaming(fetch(browser.runtime.getURL("res/origin-demangler.wasm")), {})
    .then(wasm => {
        console.log("wasm loaded");
        wasm.instance.exports.main();
    })
    .catch(err => {
        console.error("could not load wasm", err)
    });
