module.exports = function (context, timerInfo) {
    context.bindings.splunkEvent = {
        message: "Hello from a Node Function "
    };

    context.done();
}