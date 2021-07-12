const initial_state = {
    books: []
}

const BooksListReducer = (state = initial_state, action) => {
    switch (action.type.toUpperCase()) {
        case 'UPDATE_BOOKS': {
            return { ...state, books: action.books };
        }
        case 'DELETE_BOOKS': {
            return { ...state, books: [] };
        }
        default:
            return state;
    }
};

export default BooksListReducer;