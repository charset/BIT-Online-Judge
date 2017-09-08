$('#AuthorizationMode').change(function () {
    if ($('#AuthorizationMode').val() == 'Protected')
        $('#Password').removeAttr('disabled');
    else
        $('#Password').attr('disabled', 'disabled');
});