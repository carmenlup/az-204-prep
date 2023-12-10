// Check if the item contains the Quantity property and add a default property if it doesn't.'
// How to manually add the trigger. Go to the Cosmos Account -> Data Explore -> select DB -> select the container -> Add trigger and paste this code.
function validateItem() {
	var context = getContext();
	var request = context.getRequest();
	var item = request.getBody();

	if (!("Quantity" in item)) {
		item["Quantity"] = 1;
	}

	request.setBody(item);
}