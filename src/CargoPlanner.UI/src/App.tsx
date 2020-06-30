import React from 'react';
import './App.css';
import Header from './views/common/header/Header';
import Footer from './views/common/footer/Footer';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router, Route, Link, Switch } from 'react-router-dom';
import CargoList from './views/cargo-list/CargoList';
import ResultList from './views/result-list/ResultList';
import ItemEdit from './views/cargo-edit/CargoEdit';
import NotFound from './views/not-found/NotFound';
import Home from './views/home/Home';
import ViewResult from './views/view-result/ViewResult';

function App() {
  return (
    <Router>
      <Header />
      <Switch>
        <Route exact path="/" component={Home} />
        <Route exact path="/home" component={Home} />
        <Route exact path="/cargo-edit/:id" component={ItemEdit} />
        <Route exact path="/cargo-list" component={CargoList} />
        <Route exact path="/result-list" component={ResultList} />
        <Route
          exact
          path="/result/:instanceId/:resultId/:truckIndex"
          component={ViewResult}
        />
        <Route component={NotFound} />
      </Switch>
    </Router>
  );
}

export default App;
