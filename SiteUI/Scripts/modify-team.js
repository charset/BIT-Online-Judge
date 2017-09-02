
$('#addUserForm').submit(function () {
    event.preventDefault();
    $('#addUserButton').attr('disabled', 'disabled');

    var url = "/Profile/AddTeamUser?teamId=" + $('#teamIdField').val()
        + '&username=' + $('#username').val();
    $.post(url, function () {
        location.reload();
    });

    return false;
});

$('#removeUser').click(function () {
    $(this).attr('disabled', 'disabled');
    var url = '/Profile/RemoveTeamUser?teamId=' + $('#teamIdField').val()
        + '&username=' + $('#removeUser').data('username');
    $.post(url, function (data, status) {
        location.reload();
    });
});

$('#dismissTeam').click(function () {
    $(this).attr('disabled', 'disabled');
    var teamId = $('#teamIdField').val();
    var url = '/Profile/DeleteTeam?teamId=' + $('#teamIdField').val();
    $.post(url, function (data, status) {
        location.assign('/Home');
    });
});
