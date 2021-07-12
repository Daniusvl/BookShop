
const initial_state = {
    categoryName: 'Literature',
    authorName: 'William Shakespeare',
    priceMin: 1,
    priceMax: 100
};

const books_filter_reducer = (state = initial_state, action) => {
    switch (action.type.toUpperCase()) {
        case 'CHANGE_FILTER':
            return {
                categoryName: action.categoryName,
                authorName: action.authorName,
                priceMin: action.priceMin,
                priceMax: action.priceMax
            };
        default:
            return state;
    }
}

export default books_filter_reducer;