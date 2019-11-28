import React, { Component } from 'react';
import { AppConfiguration } from "read-appsettings-json";
import axios from 'axios';

export class PostEditor extends Component {
    static displayName = PostEditor.name;


    constructor(props) {
        super(props);
        this.state = { posts: [], loading: true, id: 0, notFound: true, enableErrorMessage: false, showForm: false, title: '', content: '', status: '', enablePermisionMessage: false, };
        this.url = AppConfiguration.Setting().Apiendpoint;
        this.viewPost = this.viewPost.bind(this);
        this.cancel = this.cancel.bind(this);
        this.handleUpdateStatusPost = this.handleUpdateStatusPost.bind(this);
        this.handleCallApi = this.handleCallApi.bind(this);
        this.statusPublished = 'Published';
        this.statusRejected = 'Rejected';
        this.statusDeleted = 'Deleted';
        this.userId = document.getElementById('userId').value;
    }

    componentDidMount() {
        this.populateData();
    }


    cancel() {

        this.setState({ showForm: false, title: '', content: '', id: 0 });
    }

    viewPost(e, post) {

        this.setState({ showForm: true, title: post.title, content: post.content, id: post.id, status: post.status });
    }

    handleUpdateStatusPost(event, post, status) {


        const postId = post ? post.id : this.state.id;

        this.handleCallApi(this.url + 'post/UpdateStatePost/' + postId + '/' + status);
    }


    handleCallApi(url, data) {
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'UserId': this.userId
            },
            body: data ? JSON.stringify(data) : {}
        }).then(this.fetchStatusHandler)
            .then(data => {
                this.setState({ loading: true, showForm: false, title: '', content: '', id: 0 });
                this.populateData();
            })
            .catch(error => {
                console.log(error);
            });
    }


    renderTable(posts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Action</th>
                        <th>Id</th>
                        <th>Title</th>
                        <th>Status</th>
                        <th>Created Date</th>
                    </tr>
                </thead>
                <tbody>
                    {posts.map(post =>
                        <tr key={post.id}>
                            <td>
                                <button className="btn btn-secondary" onClick={(e) => this.viewPost(e, post)}>View</button>
                                <button className="btn btn-success" onClick={(e) => this.handleUpdateStatusPost(e, post, this.statusPublished)}>Approve</button>
                                <button className="btn btn-warning" onClick={(e) => this.handleUpdateStatusPost(e, post, this.statusRejected)}>Reject</button>
                                <button className="btn btn-danger" onClick={(e) => this.handleUpdateStatusPost(e, post, this.statusDeleted)}>Delete</button>
                            </td>
                            <td>{post.id}</td>
                            <td>{post.title}</td>
                            <td>{post.status}</td>
                            <td>{post.createdDate}</td>

                        </tr>
                    )}
                </tbody>
            </table >
        );
    }


    render() {
        let contents = this.state.enableErrorMessage
            ? <p><em>Something went wrong...</em></p>
            : this.state.notFound
                ? <p><em>No records...</em></p>
                : this.state.loading
                    ? <p><em>Loading...</em></p>
                    : this.renderTable(this.state.posts);

        return (
            <div>
                <h1><span className="badge badge-secondary">Editor</span></h1>
                <div hidden={this.state.enablePermisionMessage ? 'hidden' : ''}>
                    <div hidden={this.state.showForm ? 'hidden' : ''}>
                        <p></p>
                        {contents}
                        <p></p>
                    </div>
                    <div hidden={this.state.showForm ? '' : 'hidden'}>
                        <h4>Post</h4>
                        <div className="row">
                            <div className="col-10">

                                <div className="form-group">
                                    <label htmlFor="title">Title</label>
                                    <br />
                                    <input type="text" id="title" name="title" value={this.state.title} disabled placeholder="Title" onChange={this.handleChange} />
                                </div>
                                <div className="form-group">
                                    <label htmlFor="content">Content</label>
                                    <br />
                                    <textarea id="content" name="content" rows="15" cols="155" disabled value={this.state.content} onChange={this.handleChange} >
                                    </textarea>
                                </div>
                                <div className="row">

                                    <input type="button" className="btn btn-success" onClick={(e) => this.handleUpdateStatusPost(e, null, this.statusPublished)} value="Approve" />
                                    <input type="button" className="btn btn-warning" onClick={(e) => this.handleUpdateStatusPost(e, null, this.statusRejected)} value="Reject" />
                                    <input type="button" className="btn btn-danger" onClick={(e) => this.handleUpdateStatusPost(e, null, this.statusDeleted)} value="Delete" />
                                    <input type="button" className="btn btn-secondary" onClick={this.cancel} value="Cancel" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div hidden={this.state.enablePermisionMessage ? '' : 'hidden'}>
                    You have not permission for this view!
                </div>

            </div>
        );
    }

    async populateData() {
        await this.getPosts();
    }



    async getPosts() {
        axios.get('Identity/Account/Manage').then(rolesResponse => {
            let htmLogin = rolesResponse.data;
            let begin = htmLogin.search('<roles>');
            let end = htmLogin.search('</roles>');
            let roles = htmLogin.substr(begin, end - begin).replace('<roles>', '').replace('</roles>', '');
            roles = JSON.parse(roles);
            if (roles.editor === 'False') {
                this.setState({ enablePermisionMessage: true });
            }
        }
        );

        fetch(this.url + 'post/GetPostsByStatus/Pending')
            .then(this.fetchStatusHandler)
            .then(data => {
                this.setState({ posts: data ? data : [], loading: false, notFound: data ? data.length === 0 : true, enableErrorMessage: data === null });
            })
            .catch(error => {
                this.handleError(error);
            });
    }

    fetchStatusHandler(response) {
        if (response.status === 200) {
            return response.json();
        } else if (response.status === 404) {
            return [];
        }
        else {
            console.log(response);
            return null;
        }
    }

    handleError(error) {
        this.setState({ enableErrorMessage: true });
        console.log(error);
    }
}
