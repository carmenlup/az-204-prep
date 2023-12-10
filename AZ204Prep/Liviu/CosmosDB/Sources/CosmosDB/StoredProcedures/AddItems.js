function addItems(items) {
	var context = getContext();
	var response = context.getResponse();

	var numOfItems = items.length;
	checkLength(numOfItems);

	for (let i = 0; i < numOfItems; i++) {
		createItem(items[i]);
	}


	function checkLength(itemLength) {
		if (itemLength == 0) {
			response.setBody("Error: there are no items");
			return;
		}
	}

	function createItem(item) {
		var collection = context.getCollection();
		var collectionLink = collection.getSelfLink();
		collection.createDocument(collectionLink, item);
	}
}