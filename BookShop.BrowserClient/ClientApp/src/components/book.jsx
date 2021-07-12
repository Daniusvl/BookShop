import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import useFetch from '.././hooks/use_fetch';

const Book = (props) => {
    const { id } = useParams();
    const [data, error, loading] = useFetch(`/api/books/${id}`);

    const [errorMsg, setErrorMsg] = useState('');

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [authorName, setAuthorName] = useState('');
    const [categoryName, setCategoryName] = useState('');
    const [dateReleased, setDateReleased] = useState('');
    const [price, setPrice] = useState('');

    useEffect(() => {
        if (!loading) {

            if (error) {
                setErrorMsg('Unknown error occured');
            }
            else if (data.ExceptionName == "NotFoundException") {
                setErrorMsg(data.Message);
            }
            else {
                setName(data.name);
                setDescription(data.description);
                setAuthorName(data.authorName);
                setCategoryName(data.categoryName);
                let date = new Date(data.dateReleased);
                setDateReleased(`${date.getDay()}-${date.getMonth()}-${date.getFullYear()}`);
                setPrice(data.price);
            }
        }
    });

    return (
        <div>
            <h2>
                {loading ? 'loading' : ''}
                {errorMsg}
            </h2>
            <div>
                <h2>{name}</h2>
                <b> Author: { authorName }</b> <br />
                <b> Category: {categoryName}</b><br /><br />
                <label>Description:</label> <br />
                <p>{description}</p>
                <i> Date released: { dateReleased }</i> <br />
                <b> Price: { price }</b>
            </div>
        </div>
    );
}

export default Book;