import React, {Component} from 'react';
import {Button, Input} from "reactstrap";
import {Add} from "./Add";

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            query: '',
            configs: [],
            loading: true,
            searchString: []
        };
    }

    componentDidMount() {
        this.getData();
    }


    getData = () => {

        fetch('api/DynamicConfiguration')
            .then(response => response.json())
            .then(data => {
                this.setState({
                    configs: data,
                    searchString: data
                })
            })
            .then((data) => {
                
            })
            .catch((error) => {
                
            })
    };


    render() {

        function add() {
            window.location = '/add'
        }

        function edit(key) {
            window.location = '/edit/' + key;
        }

        var content;
        if (this.state.configs.length > 0) {
            content = this.state.configs.map(config =>

                <tr>
                    <td>{config.applicationName}</td>
                    <td>{config.name}</td>
                    <td>{config.value}</td>
                    <td>{config.isActive}</td>
                    <Button style={{marginTop: "5px", marginLeft: "2px"}}
                            onClick={() => edit(config.applicationName + "." + config.name)}
                            color="success">Edit</Button>{' '}
                </tr>
            );
        }


        return (
            <div>
                <div className={"col-md-12"}>
                    <Button style={{marginTop: "5px", marginBottom: "20px"}} onClick={() => add()} color="primary">+
                        Add</Button>{' '}
                </div>
                <div className={"col-md-12"}>
                    <table className='table table-striped'>
                        <thead>
                        <tr>
                            <th>Application Name</th>
                            <th>Name</th>
                            <th>Value</th>
                            <th>Status</th>
                            <th>Action</th>
                        </tr>
                        </thead>
                        <tbody>
                        {content}


                        </tbody>
                    </table>
                </div>
            </div>
        );
    }


}
