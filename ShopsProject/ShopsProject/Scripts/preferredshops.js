function setPreferred(shopId, flag) {
    $.ajax({
        type: "POST",
        url: "/Home/SetPreferredShop",
        contentType: "application/json; charset=utf-8",
        data: '{"shopId":"' + shopId + '", "flag":"' + flag + '"}',
        dataType: "html",
        success: function (result, status, xhr) {
            document.location.reload(true);
        },
        error: function (xhr, status, error) {
        }
    });

    return false;
}