import { createStore, combineReducers } from 'redux';
import BooksListReducer from './reducers/books_list_reducer';

const store = createStore(combineReducers({
    BooksListReducer
}));

export default store;