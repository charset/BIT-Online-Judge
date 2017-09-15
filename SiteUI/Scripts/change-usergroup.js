
var form = $('#changeUsergroupForm');
if (form != null) {
    form.submit(function () {
        event.preventDefault();

        $.post(form.attr('action'), form.serialize(), function (data, status) {
            location.reload();
        });

        return false;
    });
}
