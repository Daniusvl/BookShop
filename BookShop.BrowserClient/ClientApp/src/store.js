import { createStore, combineReducers } from 'redux';
import BooksFilterReducer from './reducers/books_filter_reducer';
import BooksListReducer from './reducers/books_list_reducer';

const store = createStore(combineReducers({
    BooksFilterReducer,
    BooksListReducer
}));

export default store;