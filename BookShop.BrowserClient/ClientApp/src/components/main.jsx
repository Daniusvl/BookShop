import React from 'react';
import BookFilters from './book_filters';
import Books from './books';

const Main = (props) => {

    return (
        <div>
            <table>
                <tr>
                    <td>
                        <BookFilters />
                    </td>

                    <td>
                        <Books />
                    </td>
                </tr>
            </table>
        </div>
    );
}

export default Main;