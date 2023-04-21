import Ping from 'ping.js';
export { update_ping }

async function update_ping(hook) {
    var p = new Ping();

    p.ping(window.location.origin)
        .then(data => {
            hook.pushEvent("update_ping", { ping: data });
        })
        .catch(data => {
            console.error("Ping failed: " + data);
        })
}
