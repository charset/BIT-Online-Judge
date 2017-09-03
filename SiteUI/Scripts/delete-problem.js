
$('#deleteProblemButton').click(function () {
    $('#deleteProblemButton').attr('disabled', 'disabled');
    var url = '/Archieve/Delete?id=' + $('#problemIdField').val();
    $.post(url, function (data, status) {
        location.assign('/Archieve');
    });
});
