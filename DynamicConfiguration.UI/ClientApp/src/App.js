import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import {Add} from "./components/Add";
import {Edit} from "./components/Edit";

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/add' component={Add} />
        <Route exact path='/edit/:key' component={Edit} />
      </Layout>
    );
  }
}
