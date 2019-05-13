getLocation();
//var x = document.getElementById("demo");
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        //x.innerHTML = "Geolocation is not supported by this browser.";
    }
}
function showPosition(position) {
    //$('#hidLatitude').val(position.coords.latitude);
    //$('#hidLongitude').val(position.coords.longitude);
    setPosition(position);
}

function setPosition(position) {
    $.ajax({
        type: "POST",
        url: "/Home/SetPosition",
        contentType: "application/json; charset=utf-8",
        data: '{"latitude":"' + position.coords.latitude + '", "longitude":"' + position.coords.longitude + '"}',
        dataType: "html",
        success: function (result, status, xhr) {
        },
        error: function (xhr, status, error) {
        }
    });

    return false;
}