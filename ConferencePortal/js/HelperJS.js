
function myFunction(a) {
    var x = document.getElementById(a).value;
    $("#roomCount").val(x);
}


$("a[data-form-method='post']").click(function (event) {
    event.preventDefault();
    var element = $(this);
    var action = element.attr("href");
    element.closest("form").each(function () {
        var form = $(this);
        form.attr("action", action);
        form.submit();
    });
});