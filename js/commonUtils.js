function GetData(destination, param, callback) {
    $.ajax({
        type: "POST",
        url: destination,
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            callback(JSON.parse(msg.d));
        },
        error: function (msg) {
            window.location.href = "/error.aspx";
        }
    });
};

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}