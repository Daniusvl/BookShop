import React, { useRef, useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import Constants from '../Constants';

const BookFilters = (props) => {

    const categoryName = useRef();
    const authorName = useRef();
    const priceMin = useRef();
    const priceMax = useRef();

    useEffect(() => {
        categoryName.current.value = 'Literature';
        authorName.current.value = 'William Shakespeare';
        priceMin.current.value = 1;
        priceMax.current.value = 100;
    });

    const [loading, setLoading] = useState(false);

    const search = (event) => {
        setLoading(true);
        fetch(`${Constants.BASE_API_URL}/api/books/ByFilter`, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                categoryName: categoryName.current.value,
                authorName: authorName.current.value,
                priceMin: priceMin.current.value,
                priceMax: priceMax.current.value
            })
        }).then(resp => resp.json()).then(json => {
            props.dispatch({ type: 'update_books', books: json.map(b => <div key={b.id}><Link to={`/books/${b.id}`}>{b.name}</Link></div>) });
            setLoading(false);
        });
    };

    return (
        <div>
            Filters
            <br />
            <label>Category</label> < br/>
            <input ref={categoryName} type='text' />< br />

            <label>Author</label>< br />
            <input ref={authorName} type='text' />< br />

            <label>Min price</label>< br />
            <input ref={priceMin} type='number' />< br />

            <label>Max price</label>< br />
            <input ref={priceMax} type='number' />< br />

            <input type='button' onClick={search} value="Search" />
            <h2>
                {loading ? 'Loading' : ''}
            </h2>
        </div>
    );
}

const map_state_to_props = (state) => {
    return { state };
};

const map_dispatch_to_props = (dispatch) => {
    return { dispatch };
};

export default connect(map_state_to_props, map_dispatch_to_props)(BookFilters);