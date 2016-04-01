module.exports = function (context, timerInfo) {
    context.bindings.splunkEvent = {
        "message": "Hello from a Node function"
    };
    context.done();
}