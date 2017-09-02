
$('#createTeamButton').click(function () {
    // Validate create team form.
    var teamName = $('#teamName').val();
    if (teamName === null || teamName.length === 0)
        $('#teamNameGroup').addClass('has-error');
    else
        $('#createTeamForm').submit();
});

$('#createTeamForm').submit(function () {
    event.preventDefault();

    // AJAX Post to create a team.
    // Disable form controls.
    $('#teamName').attr('readonly', 'readonly');
    $('#createTeamButton').attr('disabled', 'disabled');

    var form = $('#createTeamForm');
    $.post(form.attr('action'), form.serialize(), function (data, status) {
        window.location.reload();
    });

    return false;
});
