function setAddPostEvent(addPostAction) {
    const addPostForm = $('#add-post-form');

    const messageInput = $('textarea[name="addPostMessage"]');
    const topicId = addPostForm.data('topic-id');

    addPostForm.submit((event) => {
        const requestData = {
            Message: messageInput.val(),
            TopicId: topicId,
        };
        
        $.ajax({
            method: "POST",
            url: addPostAction,
            data: JSON.stringify(requestData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: (response) => {
                //TODO Scroll to the end
                //Can update DOM but it would be hardcoded (not partial view) because of no model 
                //Or inconsistent with actual model
                if (response.success) {
                    const lastPostId = findLastPostId();

                    window.location += `post-${lastPostId}`;
                    window.location.reload();
                } else {
                    handleAddPostError();
                }
            },
            error: () => handleAddPostError()
        });

        event.preventDefault();
    });
}

function handleAddPostError(){

}

function findLastPostId() {
    const lastElement = $('div[id^="post-"]').last();
    const lastElementId = lastElement.attr('id');
    return lastElementId.substring("post-".length);
}