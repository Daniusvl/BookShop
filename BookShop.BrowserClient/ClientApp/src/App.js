import React, { Component } from 'react';
import { Link, Route } from 'react-router-dom';
import Main from './components/main';
import Book from './components/book';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
        <div>
            <table>
                <tr> <td>
                    <Link to="/">Home</Link>
                </td> </tr>
            </table>
            <Route exact path="/" component={Main} />
            <Route exact path="/books/:id" component={Book} />
        </div>
    );
  }
}
