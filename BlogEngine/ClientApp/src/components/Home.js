import React, { Component } from 'react';
import { AppConfiguration } from "read-appsettings-json";

export class Home extends Component {
    static displayName = Home.name;


    constructor(props) {
        super(props);
        this.state = {
            posts: [], loading: true, id: 0, notFound: true, enableErrorMessage: false, showForm: false, title: '', content: '', userId: 'User2', status: '', writerId: '', comments: [], approvalDate: '', comment: '', name: ''
        };
        this.url = AppConfiguration.Setting().Apiendpoint;
        this.viewPost = this.viewPost.bind(this);
        this.cancel = this.cancel.bind(this);
        this.saveComment = this.saveComment.bind(this);
        this.handleCallApi = this.handleCallApi.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.statusPublished = 'Published';
        this.statusRejected = 'Rejected';
        this.statusDeleted = 'Deleted';
    }

    componentDidMount() {
        this.populateData();
    }

    handleChange(event) {
        const { target: { name, value } } = event;
        this.setState({ [name]: value });
    }

    cancel() {

        this.setState({ showForm: false, title: '', content: '', id: 0 });
    }

    viewPost(e, post) {

        this.setState({ showForm: true, title: post.title, content: post.content, id: post.id, status: post.status, writerId: post.writerId, approvalDate: post.approvalDate, comments: post.comments, comment: '', name: '' });
    }

    saveComment(event) {
        event.preventDefault();
        let date = new Date(Date.now());
        const data = { postId: this.state.id, name: this.state.name, content: this.state.comment, email: '', createdDate: date.toDateString(), id: Math.floor(Math.random() * 100000) };
        let comments = this.state.comments;
        comments.push(data);
        data.id = 0;
        this.handleCallApi(this.url + 'comment/CreateComment', data, comments);
    }


    handleCallApi(url, data, comments) {
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'UserId': this.state.userId
            },
            body: data ? JSON.stringify(data) : {}
        }).then(this.fetchStatusHandler)
            .then(data => {
                this.setState({ loading: true, showForm: true, comments: comments, comment: '', name: '' });
                this.populateData();
            })
            .catch(error => {
                console.log(error);
            });
    }


    renderTable(posts) {
        return (
            <div className="panel-group main-container"> 
            <div className="row">
                    <div className="col-md-6">
                        <div>
                            {posts.map((post, index) =>
                                <div key={post.id}>{
                                    index % 2 === 0 ? (
                                        <div className="card" key={index}>
                                            <div className="card-body">
                                                <h2 className="card-title">{post.title}</h2>
                                                <h6>{post.approvalDate} </h6>
                                                <p className="card-text">{post.content.length > 250 ? post.content.substring(0, 250) + ' ...' : post.content}</p>
                                                <p className="card-text">By: {post.writerId} Comments: ({post.comments.length})</p>
                                                <input type="button" className="btn btn-primary" onClick={(e) => this.viewPost(e, post)} value="View Post" />
                                            </div>
                                        </div>
                                    ) : (<div />)}</div>)}
                        </div>
                    </div>
                    <div className="col-md-6">
                        <div>
                            {posts.map((post, index) =>
                                <div key={post.id}>{
                                    index % 2 > 0 ? (
                                        <div className="card" key={index}>
                                            <div className="card-body">
                                                <h2 className="card-title">{post.title}</h2>
                                                <h6>{post.approvalDate} </h6>
                                                <p className="card-text">{post.content.length > 250 ? post.content.substring(0, 250) + ' ...' : post.content}</p>
                                                <p className="card-text">By: {post.writerId} Comments: ({post.comments.length})</p>
                                                <input type="button" className="btn btn-primary" onClick={(e) => this.viewPost(e, post)} value="View Post" />
                                            </div>
                                        </div>
                                    ) : (<div />)}</div>)}
                        </div>
                    </div>
                </div>
            </div>
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
                <h1><span className="badge badge-secondary">Posts</span></h1>
                <div hidden={this.state.showForm ? 'hidden' : ''}>
                    <p></p>
                    {contents}
                    <p></p>
                </div>
                <div hidden={this.state.showForm ? '' : 'hidden'}>
                    <div className="row">
                        <form >
                            <div className="form-group">
                                <h2>
                                    <label htmlFor="title">{this.state.title}</label>
                                </h2>
                            </div>
                            <div className="form-group">
                                <p>
                                    <label htmlFor="content">{this.state.content}</label>
                                </p>
                            </div>
                            <div className="form-group">
                                <p>
                                    <label htmlFor="content">By: {this.state.writerId} {this.state.approvalDate}</label>
                                </p>
                            </div>
                        </form>
                    </div>
                    <div className="row">
                        <form id="post-form" method="post" onSubmit={this.saveComment}>
                            <div className="form-group">
                                <label htmlFor="title">Your Name</label>
                                <br />
                                <input type="text" id="name" name="name" value={this.state.name} placeholder="Your Name" onChange={this.handleChange} required />
                            </div>
                            <div className="form-group">
                                <label htmlFor="content">Your Comment</label>
                                <br />
                                <textarea id="comment" name="comment" rows="6" cols="155" value={this.state.comment} onChange={this.handleChange} required >
                                </textarea>
                            </div>
                            <div className="form-group">
                                <button id="save" className="btn btn-primary" >Post a comment</button>
                                <input type="button" className="btn btn-secondary" onClick={this.cancel} value="Back" />
                            </div>
                        </form>
                        <br />
                    </div>
                    <h5>Comments </h5>
                    {this.state.comments.map((comment, index) =>
                        <div className="row" key={comment.id}>
                            <div key={index}>
                                <div className="card-body">
                                    <h2 className="card-title">{comment.name}</h2>
                                    <h6>{comment.createdDate} </h6>
                                    <p className="card-text">{comment.content}</p>
                                </div>

                            </div>
                        </div>
                    )}

                    {this.state.comments.length === 0 ? <div> No comments yet! </div>:  ''}
                     
                </div>

            </div>

        );
    }

    async populateData() {
        await this.getPosts();
    }



    async getPosts() {
        fetch(this.url + 'post/GetPostsByStatus/Published')
            .then(this.fetchStatusHandler)
            .then(data => {
                for (var i = 0; i < data.length; i++) {
                    data[i].approvalDate = new Date(data[i].approvalDate).toDateString();
                    for (var j = 0; j < data[i].comments.length; j++) {
                        data[i].comments[j].createdDate = new Date(data[i].comments[j].createdDate).toDateString();
                    }
                }


                this.setState({
                    posts: data ? data : [],
                    loading: false,
                    notFound: data ? data.length === 0 : true,
                    enableErrorMessage: data === null
                });
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
