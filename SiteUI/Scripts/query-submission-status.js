
function isFinalVerdict(verdict) {
    var finals = new Array('Accepted', 'Wrong answer', 'Runtime error', 'Time limit exceeded', 'Memory limit exceeded',
        'Output limit exceeded', 'Presentation error', 'Compilation error', 'System error');

    for (item in finals) {
        if (verdict == finals[item])
            return true;
    }

    return false;
}

function getSubmissionRowClass(verdict) {
    if (isFinalVerdict(verdict)) {
        if (verdict == 'Accepted')
            return 'success';
        else if (verdict == 'Presentation error')
            return 'warning';
        else if (verdict == 'System error')
            return '';
        else
            return 'danger';
    } else
        return '';
}

function getSubmissionVerdictClass(verdict) {
    if (isFinalVerdict(verdict)) {
        if (verdict == 'Accepted')
            return 'text-verdict-ac';
        else if (verdict == 'Presentation error')
            return 'text-verdict-pe';
        else if (verdict == 'System error')
            return 'text-verdict-se';
        else
            return 'text-verdict-err';
    } else
        return 'text-verdict-unknown';
}

window.onload = function () {
    var clockId = window.setInterval(function () {
        var currentVerdict = $('#submissionVerdict').html();
        if (!isFinalVerdict(currentVerdict)) {
            // AJAX query submission state.
            $.post($('#submissionQuery').val(), function (data, status) {
                var query = JSON.parse(data);

                // Update page.
                $('#submissionVerdict').html(query.verdict);
                $('#submissionExeTime').html(query.exeTime + ' ms');
                $('#submissionExeMem').html(query.exeMem + ' KB');
                $('#submissionMessage').html(query.message);

                $('#submissionVerdict').attr('class', getSubmissionVerdictClass(query.verdict));
                $('#submissionRow').attr('class', getSubmissionRowClass(query.verdict));
            });
        } else {
            window.clearInterval(clockId);
            $('#submissionProgress').hide();
        }
    }, 1500);
};
