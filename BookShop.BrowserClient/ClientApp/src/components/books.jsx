import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';

const Books = (props) => {
    return (
        <div>
            Books
            <div>{props.state.BooksListReducer.books}</div>
        </div>    
    );
}

const map_state_to_props = (state) => {
    return { state };
};

const map_dispatch_to_props = (dispatch) => {
    return { dispatch };
};

export default connect(map_state_to_props, map_dispatch_to_props)(Books);