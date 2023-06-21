
"strict";

const memory = new WebAssembly.Memory({initial: 2, maximum: 2})

var envs = {
    "LANG" : "C.UTF_8"
}

function getenv()
{
    res = []
    for (var key in envs)
        res.push(key + "=" + envs[key])
    return res
}

function getenvdat()
{
    var total = 0;
    var env = getenv();
    env.forEach(e => total += e.length);
    total += env.length;
    return {
        n_vars : env.length,
        as_strings : env,
        total_size : total
    };
}

const envdat = getenvdat();

const imports = { 
    wasi_snapshot_preview1: {
        "environ_sizes_get" : (environ_c, environ_s) => {
            let buffer = new Uint32Array(memory.buffer);
            buffer[environ_c] = envdat.as_strings.length;
            buffer[environ_s] = envdat.total_size;
            return 0;
        },
        "environ_get": (environ_ptrs, environ_dat_ptr) => {
            let ptrbuf = new Uint32Array(memory.buffer, environ_ptrs, envdat.n_vars);
            let datbuf = new Uint8Array(memory.buffer, environ_dat_ptr, envdat.total_size)

            let offset = 0;
            envdat.as_strings.forEach((str, idx) => {
                ptrbuf[idx] = offset + environ_dat_ptr;
                for (let i = 0; i < str.length; ++i)
                    datbuf[offset + i] = str.charCodeAt(i);
                datbuf[offset + str.length] = 0;
                offset += str.length + 1;
            })

            return 0;
        }
    },
    'env': {
        'memory' : memory
    }
 }

WebAssembly.instantiateStreaming(fetch(browser.runtime.getURL("res/origin-demangler.wasm")), {})
    .then(wasm => {
        console.log("wasm loaded");
        wasm.instance.exports.main();
    })
    .catch(err => {
        console.error("could not load wasm", err)
    });
